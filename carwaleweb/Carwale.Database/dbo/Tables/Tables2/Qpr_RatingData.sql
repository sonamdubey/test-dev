CREATE TABLE [dbo].[Qpr_RatingData] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [UserId]               INT            NULL,
    [ManagerId]            INT            NULL,
    [CreatedOn]            DATETIME       CONSTRAINT [DF_Qpr_RatingData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [StartTime]            DATETIME       NULL,
    [EndTime]              DATETIME       NULL,
    [Mission]              VARCHAR (1000) NULL,
    [IsSubmitted]          BIT            NULL,
    [Type]                 SMALLINT       NULL,
    [EmployeeId]           VARCHAR (20)   NULL,
    [DesignationId]        INT            NULL,
    [DepartmentId]         INT            NULL,
    [ExtraAchieved]        VARCHAR (1000) NULL,
    [IsSingleMonthOutcome] BIT            NULL,
    CONSTRAINT [PK__Qpr_Rati__3214EC070D3F9AC5] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for self rating,2 for manager and 3 for both', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Qpr_RatingData', @level2type = N'COLUMN', @level2name = N'Type';

