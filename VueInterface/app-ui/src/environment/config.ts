export const config ={
    HOST: "daricapp://autoquit",
    PORT: 7709,
    get URL() { return `${config.HOST}:${config.PORT}`; }
}