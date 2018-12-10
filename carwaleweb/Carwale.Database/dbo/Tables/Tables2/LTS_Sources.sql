CREATE TABLE [dbo].[LTS_Sources] (
    [ID]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [ShortCode]   VARCHAR (50)  NULL,
    [CreatedBy]   BIGINT        NULL,
    [UpdatedBy]   BIGINT        NULL,
    [IsActive]    BIT           CONSTRAINT [DF_LTS_Sources_IsActive] DEFAULT ((1)) NULL,
    [UpdatedDate] DATETIME      CONSTRAINT [DF_LTS_Sources_UpdatedDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_LTS_Sources] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

