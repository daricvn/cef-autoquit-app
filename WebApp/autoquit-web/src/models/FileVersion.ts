export default class FileVersion{
    version!: string;
    links!: DownloadLink[];
}

export class DownloadLink{
    platform!: string;
    arch!: number;
    link!: string;
}