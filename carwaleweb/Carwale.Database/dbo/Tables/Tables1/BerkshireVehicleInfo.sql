CREATE TABLE [dbo].[BerkshireVehicleInfo] (
    [VEHICLE_ID]        INT           NULL,
    [VEHICLE_CODE]      BIGINT        NULL,
    [VEHICLE_TYPE_CODE] INT           NULL,
    [PRODUCT_ID]        INT           NULL,
    [VEHICLE_TYPE]      VARCHAR (300) NULL,
    [MAKE_CODE]         INT           NULL,
    [MAKE_NAME]         VARCHAR (300) NULL,
    [MODEL_CODE]        INT           NULL,
    [MODEL_NAME]        VARCHAR (300) NULL,
    [SUBTYPE_CODE]      INT           NULL,
    [SUBTYPE_NAME]      VARCHAR (600) NULL,
    [COMBINATION1]      VARCHAR (255) NULL,
    [COMBINATION2]      VARCHAR (255) NULL,
    [COMBINATION3]      VARCHAR (255) NULL,
    [COMBINATION4]      VARCHAR (255) NULL,
    [COMBINATION5]      VARCHAR (255) NULL,
    [COMBINATION6]      VARCHAR (255) NULL,
    [CUBIC_CAPACITY]    INT           NULL,
    [CARRYING_CAPACITY] INT           NULL,
    [FUEL]              CHAR (1)      NULL
);

