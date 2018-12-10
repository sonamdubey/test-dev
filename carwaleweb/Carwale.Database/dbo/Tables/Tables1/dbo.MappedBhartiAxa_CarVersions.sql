﻿CREATE TABLE [dbo].[dbo.MappedBhartiAxa_CarVersions] (
    [BhartiAxa_CarVersionsId] INT          NOT NULL,
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
    [SEGMENT]                 VARCHAR (50) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TBhartiAxa_CarVersionsV_MODEL]
    ON [dbo].[dbo.MappedBhartiAxa_CarVersions]([MODEL] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TBhartiAxa_CarVersionsV_Reference_No]
    ON [dbo].[dbo.MappedBhartiAxa_CarVersions]([Reference_No] ASC);

