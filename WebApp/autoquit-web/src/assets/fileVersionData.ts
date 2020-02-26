import FileVersion from "@/models/FileVersion";

const VersionData: FileVersion[]=[
    { version: '2.0.0', links: [
        { platform: 'Windows x86', link:'https://www.google.com', arch: 32 },
        { platform: 'Windows x64', link:'https://www.google.com', arch: 64 },
    ] },

    { version: '1.0.4 (Legacy)', links: [
        { platform: 'Windows x86', link:'https://drive.google.com/file/d/1zPtW0NxfYY7Y2UbU88rzoUYKTKYp-Vxs/view', arch: 32 },
        { platform: 'Windows x64', link:'https://drive.google.com/file/d/1H1SXVa2UYqs7BW_2nYL5dugQUo5xRQvD/view', arch: 64 },
    ] }
]

export default VersionData;