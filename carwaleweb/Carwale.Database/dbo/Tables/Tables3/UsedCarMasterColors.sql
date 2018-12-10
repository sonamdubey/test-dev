CREATE TABLE [dbo].[UsedCarMasterColors] (
    [UsedCarMasterColorsId] INT          IDENTITY (1, 1) NOT NULL,
    [ColorName]             VARCHAR (50) NULL,
    [IsDeleted]             BIT          CONSTRAINT [DF_UsedCarMasterColors_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UsedCarMasterColorsId] PRIMARY KEY CLUSTERED ([UsedCarMasterColorsId] ASC)
);

