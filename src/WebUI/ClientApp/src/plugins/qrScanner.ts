import {Ref, ref} from "vue";
import jsQR, {QRCode} from "jsqr";
import {Point} from "jsqr/dist/locator";

type Size = [number, number];

const drawLine = (
    context: CanvasRenderingContext2D,
    begin: Point,
    end: Point,
    outlineColor: string) => {
    context.beginPath();
    context.moveTo(begin.x, begin.y);
    context.lineTo(end.x, end.y);
    context.lineWidth = 3;
    context.strokeStyle = outlineColor;
    context.stroke();
}

const drawQrOutline = (
    context: CanvasRenderingContext2D,
    {location}: QRCode,
    outlineColor: string) => {
    drawLine(context, location.topLeftCorner, location.topRightCorner, outlineColor);
    drawLine(context, location.topRightCorner, location.bottomRightCorner, outlineColor);
    drawLine(context, location.bottomRightCorner, location.bottomLeftCorner, outlineColor);
    drawLine(context, location.bottomLeftCorner, location.topLeftCorner, outlineColor);
}

// TODO optimise
const useQrScanner = (
    stream: Ref<MediaStream | undefined>,
    scaling?: Ref<number | "fit">,
    outlineColor?: string) => {
    const data = ref<string | undefined>();
    const active = ref(false);

    // https://github.com/cozmo/jsQR/blob/master/docs/index.html
    const video = ref(document.createElement("video"));
    video.value.setAttribute("playsinline", "playsinline");

    const container = ref<HTMLElement | undefined>(undefined);
    const canvas = ref<HTMLCanvasElement | undefined>(undefined);
    const containerProvider = (c: HTMLElement) => {
        if (canvas.value) {
            return;
        }

        container.value = c;
        canvas.value = document.createElement("canvas");
        canvas.value.setAttribute("hidden", "hidden");
        container.value.appendChild(canvas.value);
    };

    const start = () => {
        if (!stream.value) {
            return;
        }

        active.value = true;
        canvas.value?.removeAttribute("hidden");
        video.value.srcObject = stream.value;
        video.value.play().then(() => requestAnimationFrame(update));
    };

    const stop = () => {
        active.value = false;
        canvas.value?.setAttribute("hidden", "hidden");
        // do or do not, there is no stop() method for video elements
        video.value.pause();
        video.value.currentTime = 0;
    }

    const update = () => {
        if (!(active.value && video.value.readyState === HTMLMediaElement.HAVE_ENOUGH_DATA)) {
            return;
        }

        // draw video
        const context = canvas.value?.getContext("2d")!;
        const [width, height] = calcSize();

        canvas.value!.width = width;
        canvas.value!.height = height;
        context!.drawImage(video.value, 0, 0, width, height);

        // find QR
        const imageData = context.getImageData(0, 0, width, height);
        const scan = jsQR(imageData.data, imageData.width, imageData.height, {inversionAttempts: "dontInvert"});

        // draw QR
        if (scan) {
            data.value = scan.data;
            outlineColor && drawQrOutline(context, scan, outlineColor);
        }

        requestAnimationFrame(update);
    };

    const calcSize = (): Size => {
        const size: Size = [video.value.videoWidth, video.value.videoHeight];

        if (!scaling) {
            return size;
        }

        const scale = scaling.value === "fit"
            ? container.value!.getBoundingClientRect().width / video.value.videoWidth
            : scaling.value;

        return [size[0] * scale, size[1] * scale];
    };

    return {containerProvider, data, start, stop};
};

export {useQrScanner}