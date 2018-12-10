CREATE TABLE [dbo].[CRM_BookingCalls] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [CallId]     BIGINT   NOT NULL,
    [TaggedBy]   INT      NOT NULL,
    [TaggedDate] DATETIME CONSTRAINT [DF_CRM_BookingCalls_TaggedDate] DEFAULT (getdate()) NOT NULL
);

