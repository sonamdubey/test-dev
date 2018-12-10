CREATE TABLE [dbo].[BA_AppVersion] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Version]      VARCHAR (20) NOT NULL,
    [StartDate]    DATETIME     NULL,
    [EndDate]      DATETIME     CONSTRAINT [DF_BA_AppVersion_EndDate] DEFAULT ((0)) NULL,
    [IsSupported]  BIT          NOT NULL,
    [IsCompatible] BIT          CONSTRAINT [DF_BA_AppVersion_IsCompatible] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_BA_AppVersion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

