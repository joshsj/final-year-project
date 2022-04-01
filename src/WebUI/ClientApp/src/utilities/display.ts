const dateFormats = {
    full: "toLocaleString",
    date: "toLocaleDateString",
    time: "toLocaleTimeString",
} as const;

const display = {
    date: (d: Date, as: keyof typeof dateFormats = "full") : string => d[dateFormats[as]]("en-GB"),
    bool: (b : boolean) : string => b ? "Yes" : "No"
};

export {display}