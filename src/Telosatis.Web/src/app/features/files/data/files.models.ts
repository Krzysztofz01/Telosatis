export enum ListDirectoryOrderKey {
    Name = 1,
    Size = 2,
};

export interface FileInfoDto {
    name: string;
    path: string;
    size: number;
    contentType: string;
    isDir: boolean;
};

export interface MoveFileDto {
    sourceFilePath: string;
    destinationDirectoryPath: string;
};

export type UploadState<T> =
    | { status: 'hashing'; percent: number }
    | { status: 'uploading'; percent: number }
    | { status: 'done'; data: T };

