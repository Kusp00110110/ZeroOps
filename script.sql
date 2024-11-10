IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AiOperationsConfiguration] (
    [Id] int NOT NULL IDENTITY,
    [ApiKey] nvarchar(1024) NOT NULL,
    [ModelName] nvarchar(1024) NOT NULL,
    [RandToUsdExchangeRate] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_AiOperationsConfiguration] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AiOpsProductSchemaResultModels] (
    [Id] int NOT NULL IDENTITY,
    [ItemNumber] nvarchar(1024) NULL,
    [ProductManufacturerBrandName] nvarchar(1024) NULL,
    [ProductName] nvarchar(1024) NULL,
    [ProductDescription] nvarchar(max) NULL,
    [ProductCategoriesJson] nvarchar(max) NULL,
    [ProductSpecification] nvarchar(max) NULL,
    [ProductLengthMeters] decimal(18,2) NULL,
    [ProductWidthMeters] decimal(18,2) NULL,
    [ProductHeightMeters] decimal(18,2) NULL,
    [ProductWeightKg] decimal(18,2) NULL,
    [DuckDuckGoImageApiSearchString] nvarchar(500) NULL,
    CONSTRAINT [PK_AiOpsProductSchemaResultModels] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AudioSurePriceListLoadItems] (
    [Id] int NOT NULL IDENTITY,
    [MANUFACTURER] nvarchar(1024) NOT NULL,
    [FormattedItemNumber] nvarchar(1024) NULL,
    [ItemDescription] nvarchar(1024) NULL,
    [Category] nvarchar(1024) NULL,
    [StockUnitofMeasure] nvarchar(1024) NULL,
    [UNITPRICE] decimal(18,2) NOT NULL,
    [INSTOCKSTATUS] nvarchar(1024) NULL,
    CONSTRAINT [PK_AudioSurePriceListLoadItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CacheConfiguration] (
    [Id] int NOT NULL IDENTITY,
    [LocalFileStorageLocation] nvarchar(1024) NOT NULL,
    [CreateIfNotExists] bit NULL,
    CONSTRAINT [PK_CacheConfiguration] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DocumentStoreItems] (
    [Id] int NOT NULL IDENTITY,
    [DocumentName] nvarchar(1024) NOT NULL,
    [JsonContent] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_DocumentStoreItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ImageApiResponses] (
    [Id] int NOT NULL IDENTITY,
    [AiOpsProductSchemaResultModelId] int NOT NULL,
    [Height] int NOT NULL,
    [Image] nvarchar(1024) NULL,
    [ImageToken] nvarchar(1024) NULL,
    [Source] nvarchar(1024) NULL,
    [Thumbnail] nvarchar(1024) NULL,
    [ThumbnailToken] nvarchar(1024) NULL,
    [Title] nvarchar(1024) NULL,
    [Url] nvarchar(1024) NULL,
    [Width] int NOT NULL,
    CONSTRAINT [PK_ImageApiResponses] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [MdPriceListLoadItems] (
    [Id] int NOT NULL IDENTITY,
    [Brand] nvarchar(1024) NOT NULL,
    [ItemCode] nvarchar(1024) NOT NULL,
    [Description] nvarchar(1024) NULL,
    [ListPrice] decimal(18,2) NOT NULL,
    [WHCPT] decimal(18,2) NOT NULL,
    [WHGAU] decimal(18,2) NOT NULL,
    [GrandTotal] int NOT NULL,
    CONSTRAINT [PK_MdPriceListLoadItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PriceListLoadItems] (
    [Id] int NOT NULL IDENTITY,
    [Brand] nvarchar(100) NOT NULL,
    [ItemNumber] nvarchar(50) NOT NULL,
    [Description] nvarchar(max) NULL,
    [QuantityOnHand] int NOT NULL,
    [RecommendedRetailPrice] decimal(18,2) NOT NULL,
    [HasHeader] bit NOT NULL,
    [HeaderRowNumber] int NOT NULL,
    [SheetName] nvarchar(max) NULL,
    [HeaderStartColumn] int NOT NULL,
    [HeaderEndColumn] int NOT NULL,
    [SupplierCode] nvarchar(50) NULL,
    CONSTRAINT [PK_PriceListLoadItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RockitPriceListLoadItems] (
    [Id] int NOT NULL IDENTITY,
    [Brand] nvarchar(1024) NOT NULL,
    [Code] nvarchar(1024) NOT NULL,
    [Description] nvarchar(1024) NULL,
    [SRPIncVat] decimal(18,2) NOT NULL,
    [HasHeader] bit NOT NULL,
    CONSTRAINT [PK_RockitPriceListLoadItems] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241104102217_Initial', N'8.0.10');
GO

COMMIT;
GO

