CREATE TABLE [dbo].[TC_APIUsers] (
    [DealerId]          NUMERIC (18) NOT NULL,
    [UserId]            VARCHAR (50) NOT NULL,
    [Password]          VARCHAR (50) NOT NULL,
    [IsActive]          BIT          CONSTRAINT [DF_TC_APIUsers_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]         DATETIME     CONSTRAINT [DF_TC_APIUsers_EntryDate] DEFAULT (getdate()) NOT NULL,
    [LastRequestedTime] DATETIME     NULL,
    CONSTRAINT [u_dealerId] UNIQUE NONCLUSTERED ([DealerId] ASC),
    CONSTRAINT [u_userId] UNIQUE NONCLUSTERED ([UserId] ASC)
);

