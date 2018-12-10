CREATE TABLE [dbo].[Classified_CarColors] (
    [id]         SMALLINT     IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ColorName]  VARCHAR (50) NOT NULL,
    [OrderName]  VARCHAR (50) NOT NULL,
    [HexCode]    VARCHAR (6)  NOT NULL,
    [IsExterior] BIT          CONSTRAINT [DF_Classified_CarColors_IsExterior] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Classified_CarColors] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

