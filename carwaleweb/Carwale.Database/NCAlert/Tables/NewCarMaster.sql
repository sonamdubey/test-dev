CREATE TABLE [NCAlert].[NewCarMaster] (
    [MakeId]           INT           NULL,
    [MakeName]         VARCHAR (50)  NULL,
    [ModelId]          INT           NULL,
    [ModelName]        VARCHAR (50)  NULL,
    [ModelMaskingName] VARCHAR (50)  NULL,
    [VersionId]        INT           NOT NULL,
    [VersionName]      VARCHAR (50)  NULL,
    [ImageUrl]         VARCHAR (250) NULL,
    [ExShowroomPrice]  INT           NULL,
    [RTO]              INT           NULL,
    [Insurance]        INT           NULL,
    [DepotCharges]     INT           NULL,
    CONSTRAINT [PK_NewCarMaster_VersionId] PRIMARY KEY CLUSTERED ([VersionId] ASC)
);

