CREATE TABLE [dbo].[TC_TargetType] (
    [TC_TargetTypeId] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [TargetType]      VARCHAR (75) NULL,
    [TC_PanelTypeId]  TINYINT      NULL,
    [IsActive]        BIT          CONSTRAINT [DF_TC_TargetType_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK__TC_Targe__31C652085BC83AC2] PRIMARY KEY CLUSTERED ([TC_TargetTypeId] ASC)
);

