CREATE TABLE [dbo].[MyCarwale_TGVotes] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MyGarageId]   NUMERIC (18) NULL,
    [KumbhMela]    VARCHAR (50) NULL,
    [BrahmaTemple] VARCHAR (50) NULL,
    [LakeCity]     VARCHAR (50) NULL,
    CONSTRAINT [PK_MyCarwale_TGVotes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

