CREATE TABLE [CRM].[LS_PQScore] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [PQ_Id]     NUMERIC (18) NULL,
    [CBD_Id]    NUMERIC (18) NULL,
    [LeadId]    NUMERIC (18) NULL,
    [Score]     FLOAT (53)   NULL,
    [CreatedOn] DATETIME     NULL
);

