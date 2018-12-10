CREATE TABLE [dbo].[MyCarwaleReminders] (
    [ID]                        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MyCarwaleCarId]            NUMERIC (18) NOT NULL,
    [LastInsuranceReminderDate] DATETIME     NULL,
    [LastWarrantyReminderDate]  DATETIME     NULL,
    [LastPUCReminderDate]       DATETIME     NULL,
    [LastServiceReminderDate]   DATETIME     NULL,
    [LastOilChangeReminderDate] DATETIME     NULL,
    [LastUpdateReminderDate]    DATETIME     NULL,
    CONSTRAINT [PK_MyCarwaleReminders] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

