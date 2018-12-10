CREATE TABLE [dbo].[CL_ExtensionMap] (
    [Extension]      NUMERIC (18) NOT NULL,
    [UserId]         NUMERIC (18) CONSTRAINT [DF_CL_ExtensionMap_UserId] DEFAULT ((-1)) NOT NULL,
    [DialerType]     SMALLINT     CONSTRAINT [DF_CL_ExtensionMap_DialerType] DEFAULT ((1)) NOT NULL,
    [OfficeId]       SMALLINT     CONSTRAINT [DF_CL_ExtensionMap_OfficeId] DEFAULT ((1)) NOT NULL,
    [DrishtiLoginId] INT          NULL,
    CONSTRAINT [PK_CL_ExtensionMap] PRIMARY KEY CLUSTERED ([Extension] ASC) WITH (FILLFACTOR = 90)
);

