IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_UpdateTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_UpdateTarget]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 16 Oct 2013
-- Description : Save or update details of BusinessUnit into CW_TargetAchievement 
-- =============================================
 CREATE PROCEDURE [dbo].[CMS_UpdateTarget]
 (
	@BussinessUnitId        SmallInt,
	@WeekNumber             INT,
	@StartDate              DateTime,
	@EndDate                DateTime,
	@Target                 INT,
	@Achieved               INT,
	@MonthNumber            SMALLINT,
	@MonthName              VARCHAR(20),
	@Year                   SMALLINT,
	@UserId                 INT,
	@Date                   DateTime
 )
 AS
   BEGIN    
       UPDATE CW_TargetAchievement SET  Achieved=@Achieved, UpdatedBy=@UserId,UpdatedOn=@Date
	   WHERE  WeekNumber= @WeekNumber AND Year=@Year AND MonthNumber=@MonthNumber AND BU_Id=@BussinessUnitId

       IF @@ROWCOUNT = 0 
		   BEGIN
			  INSERT INTO CW_TargetAchievement(BU_Id,WeekNumber,WkStartDate,WkEndDate,Target,Achieved,MonthNumber,MonthName,Year,EntryBy,EntryDate,UpdatedBy,UpdatedOn) 
			  VALUES(@BussinessUnitId,@WeekNumber,@StartDate,@EndDate,@Target,@Achieved,@MonthNumber,@MonthName,@Year,@UserId,@Date,@UserId,@Date)
		   END
  END