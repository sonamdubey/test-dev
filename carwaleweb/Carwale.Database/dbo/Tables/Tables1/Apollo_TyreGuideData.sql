CREATE TABLE [dbo].[Apollo_TyreGuideData] (
    [ID]                 INT            NULL,
    [Car]                NVARCHAR (255) NULL,
    [Make]               NVARCHAR (255) NULL,
    [Model]              NVARCHAR (255) NULL,
    [Version]            NVARCHAR (255) NULL,
    [Tyres]              NVARCHAR (255) NULL,
    [TyreOption1]        NVARCHAR (255) NULL,
    [IsOption1Available] BIT            NULL,
    [TyreOption2]        NVARCHAR (255) NULL,
    [IsOption2Available] BIT            NULL,
    [TyreOption3]        NVARCHAR (255) NULL,
    [IsOption3Available] BIT            NULL,
    [ImgTyreOption1]     NVARCHAR (255) NULL,
    [ImgTyreOption2]     NVARCHAR (255) NULL,
    [ImgTyreOption3]     NVARCHAR (255) NULL
);

