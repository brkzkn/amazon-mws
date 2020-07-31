CREATE TABLE [dbo].[Product_Price] (
    [Id]                         INT        IDENTITY (1, 1) NOT NULL,
    [Product_Id]                 INT        NOT NULL,
    [Store_Id]                   INT        NOT NULL,
    [Min_New_ISBN_Price_Dollar]  SMALLMONEY NULL,
    [Min_Used_ISBN_Price_Dollar] SMALLMONEY NULL,
    [Min_New_ISBN_Price_Pound]   SMALLMONEY NULL,
    [Min_Used_ISBN_Price_Pound]  SMALLMONEY NULL,
    [Min_New_ASIN_Price_Dollar]  SMALLMONEY NULL,
    [Min_Used_ASIN_Price_Dollar] SMALLMONEY NULL,
    [Min_New_ASIN_Price_Pound]   SMALLMONEY NULL,
    [Min_Used_ASIN_Price_Pound]  SMALLMONEY NULL,
    [Is_Fixed_New_Dollar]        BIT        NULL,
    [Is_Fixed_Used_Dollar]       BIT        NULL,
    [Is_Fixed_New_Pound]         BIT        NULL,
    [Is_Fixed_Used_Pound]        BIT        NULL,
    CONSTRAINT [PK_Product_Price] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Product_Price_Product] FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_Product_Price_Store] FOREIGN KEY ([Store_Id]) REFERENCES [dbo].[Store] ([Id])
);



