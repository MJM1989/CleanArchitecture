CREATE TABLE [dbo].[_migrations]
(
    [Name]       nvarchar(100) NOT NULL,
    [ExecutedOn] datetime2(0)  NOT NULL,
    CONSTRAINT [PK__migrations_Name] PRIMARY KEY ([Name])
)

CREATE TABLE [dbo].[ApplicationUsers]
(
    [Id]                 uniqueidentifier NOT NULL PRIMARY KEY ROWGUIDCOL DEFAULT (NEWID()),
    [UserName]           nvarchar(256)    NOT NULL,
    [NormalizedUserName] nvarchar(256)    NOT NULL,
    [Email]              nvarchar(256)    NULL,
    [NormalizedEmail]    nvarchar(256)    NULL,
    [EmailConfirmed]     bit              NOT NULL,
    [PasswordHash]       nvarchar(max)    NULL,
    INDEX [IX_ApplicationUsers_NormalizedUserName] ([NormalizedUserName])
)


CREATE TABLE [dbo].[ApplicationRoles]
(
    [Id]             uniqueidentifier NOT NULL PRIMARY KEY ROWGUIDCOL DEFAULT (NEWID()),
    [Name]           nvarchar(256)    NOT NULL,
    [NormalizedName] nvarchar(256)    NOT NULL,
    INDEX [IX_ApplicationRoles_NormalizedName] ([NormalizedName])
)

CREATE TABLE [dbo].[ApplicationUserRoles]
(
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_ApplicationUserRoles_User_ApplicationUsers_Id] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUsers] ([Id]),
    CONSTRAINT [FK_ApplicationUserRoles_Role_ApplicationRoles_Id] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRoles] ([Id])
)
