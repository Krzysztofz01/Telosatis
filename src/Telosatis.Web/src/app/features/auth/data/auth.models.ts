export interface AuthenticateDto {
    secret: string;
};

export interface AuthenticatedDto {
    bearerToken: string;
    expiresAt: string;
};

