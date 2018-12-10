CREATE TABLE [dbo].[AutoFriendCarMap] (
    [AutoFriendCar]    VARCHAR (100) NOT NULL,
    [CarwaleVersionId] NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_AutoFriendCarMap] PRIMARY KEY CLUSTERED ([AutoFriendCar] ASC, [CarwaleVersionId] ASC) WITH (FILLFACTOR = 90)
);

