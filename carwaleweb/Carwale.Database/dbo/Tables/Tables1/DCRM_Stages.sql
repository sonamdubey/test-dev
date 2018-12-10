CREATE TABLE [dbo].[DCRM_Stages] (
    [StageId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [Descr]   VARCHAR (50) NULL,
    CONSTRAINT [PK_DCRM_Stages] PRIMARY KEY CLUSTERED ([StageId] ASC)
);

