CREATE TABLE [dbo].[Criterion] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Store_Id] INT            NOT NULL,
    [Key]      VARCHAR (100)  NOT NULL,
    [Value]    VARCHAR (5000) NOT NULL,
    CONSTRAINT [PK_Criteria] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Criterion_Store] FOREIGN KEY ([Store_Id]) REFERENCES [dbo].[Store] ([Id])
);

