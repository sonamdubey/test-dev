CREATE TYPE [dbo].[LeadScore] AS TABLE (
    [LeadId]     BIGINT     NULL,
    [CBDId]      BIGINT     NULL,
    [PQId]       BIGINT     NULL,
    [CategoryId] INT        NULL,
    [Score]      FLOAT (53) NULL);

