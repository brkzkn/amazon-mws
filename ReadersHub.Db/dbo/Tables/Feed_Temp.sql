CREATE TABLE [dbo].[Feed_Temp] (
    [Id]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [SKU]        VARCHAR (50) NOT NULL,
    [ASIN]       VARCHAR (50) NOT NULL,
    [Condition]  VARCHAR (10) NOT NULL,
    [Price]      SMALLMONEY   NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [Status]     VARCHAR (50) NOT NULL,
    [Seller_Id]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Feed_Temp] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Feed_Temp]
    ON [dbo].[Feed_Temp]([Seller_Id] ASC);

