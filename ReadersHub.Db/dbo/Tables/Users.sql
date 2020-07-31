CREATE TABLE [dbo].[Users] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Username]        VARCHAR (50)  NOT NULL,
    [Password]        VARCHAR (255) NULL,
    [Full_Name]       VARCHAR (100) NULL,
    [Email]           VARCHAR (150) NULL,
    [Registered_Date] DATETIME      NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

