IF DB_ID('UrlShortenerDb') IS NULL
    CREATE DATABASE UrlShortenerDb;
GO

USE UrlShortenerDb;
GO

IF OBJECT_ID('dbo.ShortUrls', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ShortUrls
    (
        Id BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ShortUrls PRIMARY KEY,
        ShortCode VARCHAR(20) NOT NULL,
        OriginalUrl NVARCHAR(2048) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL,
        ExpiresAtUtc DATETIME2 NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_ShortUrls_IsActive DEFAULT 1,
        ClickCount BIGINT NOT NULL CONSTRAINT DF_ShortUrls_ClickCount DEFAULT 0,
        CONSTRAINT UQ_ShortUrls_ShortCode UNIQUE (ShortCode)
    );
END
GO

IF OBJECT_ID('dbo.ClickEvents', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ClickEvents
    (
        Id BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ClickEvents PRIMARY KEY,
        ShortUrlId BIGINT NOT NULL,
        ClickedAtUtc DATETIME2 NOT NULL,
        IpAddressHash VARCHAR(64) NULL,
        UserAgent NVARCHAR(1000) NULL,
        Referrer NVARCHAR(2048) NULL,
        CONSTRAINT FK_ClickEvents_ShortUrls
            FOREIGN KEY (ShortUrlId) REFERENCES dbo.ShortUrls(Id)
            ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_ClickEvents_ShortUrlId_ClickedAt'
      AND object_id = OBJECT_ID('dbo.ClickEvents')
)
BEGIN
    CREATE INDEX IX_ClickEvents_ShortUrlId_ClickedAt
        ON dbo.ClickEvents(ShortUrlId, ClickedAtUtc);
END
GO