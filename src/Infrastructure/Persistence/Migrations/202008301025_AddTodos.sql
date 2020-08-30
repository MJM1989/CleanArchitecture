CREATE TABLE [TodoLists]
(
    [Id]             int IDENTITY
        CONSTRAINT [PK_TodoLists_Id]
            PRIMARY KEY NONCLUSTERED,
    [Title]          nvarchar(250)    NOT NULL,
    [Colour]         nvarchar(100),
    [CreatedBy]      uniqueidentifier NOT NULL,
    [Created]        datetime2(3)     NOT NULL,
    [LastModifiedBy] uniqueidentifier,
    [LastModified]   datetime2(3),
)

CREATE UNIQUE INDEX [IX_TodoLists_Title] ON [TodoLists] ([Title])

CREATE TABLE [TodoItems]
(
    [Id]             int IDENTITY
        CONSTRAINT [PK_TodoItems_Id]
            PRIMARY KEY NONCLUSTERED,
    [TodoListId]     int              NOT NULL
        CONSTRAINT [FK_TodoItems_TodoListId_TodoLists_Id]
            REFERENCES [TodoLists] ([Id]),
    [Title]          nvarchar(250)    NOT NULL,
    [Note]           nvarchar(1000),
    [Done]           bit DEFAULT 0    NOT NULL,
    [Reminder]       datetime2(0),
    [Priority]       tinyint          NOT NULL,
    [CreatedBy]      uniqueidentifier NOT NULL,
    [Created]        datetime2(3)     NOT NULL,
    [LastModifiedBy] uniqueidentifier,
    [LastModified]   datetime2(3),
)

CREATE UNIQUE INDEX [IX_TodoItems_Title] ON [TodoItems] ([Title])