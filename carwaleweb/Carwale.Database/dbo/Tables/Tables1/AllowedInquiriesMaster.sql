CREATE TABLE [dbo].[AllowedInquiriesMaster] (
    [ConsumerType]   SMALLINT NOT NULL,
    [MaxPaidInquiry] INT      NOT NULL,
    [MaxFreeInquiry] INT      NOT NULL,
    CONSTRAINT [PK_AllowedInquiriesMaster] PRIMARY KEY CLUSTERED ([ConsumerType] ASC) WITH (FILLFACTOR = 90)
);

