CREATE TABLE [dbo].[TC_PQComponents] (
    [TC_PQComponentsId] TINYINT      NOT NULL,
    [Name]              VARCHAR (50) NOT NULL,
    [IsActive]          BIT          NOT NULL,
    [Action]            TINYINT      NULL,
    [IsManditory]       BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TC_PQComponents] PRIMARY KEY CLUSTERED ([TC_PQComponentsId] ASC)
);

