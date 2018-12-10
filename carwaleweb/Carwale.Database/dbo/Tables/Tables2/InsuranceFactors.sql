CREATE TABLE [dbo].[InsuranceFactors] (
    [MakeId] NUMERIC (18)    NOT NULL,
    [A1]     DECIMAL (18, 3) NULL,
    [A2]     DECIMAL (18, 3) NULL,
    [A3]     DECIMAL (18, 3) NULL,
    [B1]     DECIMAL (18, 3) NULL,
    [B2]     DECIMAL (18, 3) NULL,
    [B3]     DECIMAL (18, 3) NULL,
    CONSTRAINT [PK_InsuranceFactors] PRIMARY KEY CLUSTERED ([MakeId] ASC) WITH (FILLFACTOR = 90)
);

