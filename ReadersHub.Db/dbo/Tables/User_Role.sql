CREATE TABLE [dbo].[User_Role] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [User_Id]   INT          NOT NULL,
    [Role_Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_User_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Role_Users] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([Id])
);

