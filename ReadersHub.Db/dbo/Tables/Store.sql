CREATE TABLE [dbo].[Store] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [SellerId]      VARCHAR (50)  NOT NULL,
    [MarketPlaceId] VARCHAR (50)  NOT NULL,
    [CurrencyCode]  VARCHAR (3)   NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED ([Id] ASC)
);

