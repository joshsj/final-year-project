const dateFormats = {
    full: "toLocaleString",
    date: "toLocaleDateString",
    time: "toLocaleTimeString",
} as const;

const display = {
    date: (d: Date, as: keyof typeof dateFormats = "full") => d[dateFormats[as]]("en-GB")
};

export {display}