export enum TokenRoleType {
    RootAdmin = 1,
    Admin = 2,
};

export interface CreateTokenDto {
    name: string;
    roleType: TokenRoleType;
    expiresAt: string;
};

export interface TokenDto {
    id: string;
    name: string;
    roleType: TokenRoleType;
    createdAt: string;
    expiresAt: string | null;
}

export interface TokenCreatedDto {
    token: TokenDto;
    secret: string;
}

export interface TokenRootAdminCreatedDto {
    created: boolean;
}

export enum ListTokensOrderKey {
    Id = 1,
    Name = 2,
    RoleType = 3,
    ExpiresAt = 4,
    CreatedAt = 5,
}