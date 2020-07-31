CREATE TABLE [dbo].[Product] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [ISBN]                 VARCHAR (20)   NOT NULL,
    [ISBN_Name]            VARCHAR (1500) NOT NULL,
    [ASIN]                 VARCHAR (20)   NOT NULL,
    [ASIN_Name]            VARCHAR (1500) NOT NULL,
    [Price_Update_Time_UK] DATETIME       NULL,
    [Price_Update_Time_US] DATETIME       NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);







