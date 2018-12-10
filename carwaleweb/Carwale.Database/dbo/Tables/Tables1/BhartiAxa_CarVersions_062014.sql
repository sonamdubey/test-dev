CREATE TABLE [dbo].[BhartiAxa_CarVersions_062014] (
    [BhartiAxa_CarVersionsId] INT          IDENTITY (1, 1) NOT NULL,
    [ITEM_CODE]               VARCHAR (10) NULL,
    [MANUFACTURE]             VARCHAR (60) NULL,
    [PRODUCT_TYPE]            VARCHAR (10) NULL,
    [MODEL]                   VARCHAR (40) NULL,
    [VARIANT]                 VARCHAR (50) NULL,
    [Reference_No]            VARCHAR (10) NULL,
    [EX_SHOWROOM]             BIGINT       NULL,
    [CC]                      SMALLINT     NULL,
    [SEAT_CAPCITY]            TINYINT      NULL,
    [FUEL]                    VARCHAR (10) NULL,
    [TONNAGE]                 VARCHAR (50) NULL,
    [SEGMENT]                 VARCHAR (50) NULL,
    CONSTRAINT [PK_BhartiAxa_CarVersionsId] PRIMARY KEY CLUSTERED ([BhartiAxa_CarVersionsId] ASC)
);

