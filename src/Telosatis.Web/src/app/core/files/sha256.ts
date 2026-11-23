import { createSHA256 } from 'hash-wasm';

export async function sha256Hex(
    file: Blob,
    onProgress?: (percent: number) => void,
    signal?: AbortSignal,
) : Promise<string> {
    const hasher = await createSHA256();
    hasher.init();

    const reader = file.stream().getReader();
    let read = 0;
    for (;;) {
        signal?.throwIfAborted();

        const { done, value } = await reader.read();
        if (done) break;

        hasher.update(value);
        read += value.byteLength;
        
        onProgress?.(Math.round((100 * read) / file.size));
    }

    return hasher.digest('hex');
}