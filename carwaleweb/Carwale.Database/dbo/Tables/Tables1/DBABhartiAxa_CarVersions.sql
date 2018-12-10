CREATE TABLE [dbo].[DBABhartiAxa_CarVersions] (
    [Reference_No]            INT          IDENTITY (2859, 1) NOT NULL,
    [BhartiAxa_CarVersionsId] INT          NULL,
    [ITEM_CODE]               VARCHAR (10) NULL,
    [MANUFACTURE]             VARCHAR (60) NULL,
    [PRODUCT_TYPE]            VARCHAR (10) NULL,
    [MODEL]                   VARCHAR (40) NULL,
    [VARIANT]                 VARCHAR (50) NULL,
    [EX_SHOWROOM]             BIGINT       NULL,
    [CC]                      SMALLINT     NULL,
    [SEAT_CAPCITY]            TINYINT      NULL,
    [FUEL]                    VARCHAR (10) NULL,
    [TONNAGE]                 VARCHAR (50) NULL,
    [SEGMENT]                 VARCHAR (50) NULL,
    [CWVersionId]             SMALLINT     NULL
);

