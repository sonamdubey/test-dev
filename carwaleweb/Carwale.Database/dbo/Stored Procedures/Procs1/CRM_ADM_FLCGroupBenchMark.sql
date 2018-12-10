IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_FLCGroupBenchMark]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_FLCGroupBenchMark]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 05 AUG 2013
-- Description:	This Proc. Insert Group BenchMark Info into CRM_ADM_GroupBenchMark 
--            : This Proc. Used in AddFLCGroupBenchMark.cs
-- =============================================

CREATE PROCEDURE [dbo].[CRM_ADM_FLCGroupBenchMark]
	@GroupId INT,
	@BenchMark	FLOAT,
	@CreatedBy	NUMERIC(18,0)
	 
AS
declare @BenchMarkValue numeric(18,2)
	BEGIN
	     set @BenchMarkValue =cast (@BenchMark as numeric(18,4))
		--INSERT STATMENT
		INSERT INTO CRM_ADM_GroupBenchMark(GroupId, BenchMark, CreatedBy,CreatedOn)
		VALUES(@GroupId, @BenchMarkValue, @CreatedBy,GETDATE())	
    END
