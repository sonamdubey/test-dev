CREATE TABLE [dbo].[AE_CarCondition] (
    [CarId]        NUMERIC (18) NOT NULL,
    [Interior]     SMALLINT     NULL,
    [Exterior]     SMALLINT     NULL,
    [Engine]       SMALLINT     NULL,
    [TYRE]         SMALLINT     NULL,
    [Transmission] SMALLINT     NULL,
    [Electrical]   SMALLINT     NULL,
    [Seat]         SMALLINT     NULL,
    [Brakes]       SMALLINT     NULL,
    [Suspension]   SMALLINT     NULL,
    [Steering]     SMALLINT     NULL,
    [UpdatedOn]    DATETIME     NULL,
    [UpdatedBy]    NUMERIC (18) NULL,
    CONSTRAINT [PK_AE_CarCondition] PRIMARY KEY CLUSTERED ([CarId] ASC) WITH (FILLFACTOR = 90)
);

