IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CarBookingAssignTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CarBookingAssignTarget]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 1st March 2014
-- Description : Save Car Booking Target
-- Modifier	   : Ruchira Patil on 17th July 2014 (inserting the target and date into CRM_Targets by fetching it from table type [dbo].[CRM_Targets])
-- =============================================
CREATE PROCEDURE [dbo].[CRM_CarBookingAssignTarget]
(
@Type        INT,
@Targets	 [dbo].[CRM_Targets] READONLY,
@Brand       INT,
@CreatedOn   DATETIME,
@Status		 BIT OUTPUT
)
AS
   BEGIN   
		DECLARE @NumberOfTargets INT = 0,
				@i				 TINYINT,
				@TempDate		 DATETIME,
				@TempTarget		 INT

		SELECT @NumberOfTargets = COUNT(*) FROM @Targets
		SET @i = 1

		WHILE @NumberOfTargets > 0
		BEGIN
			SELECT @TempDate=TG_Date, @TempTarget=Targets FROM @Targets WHERE Id = @i

			SELECT ID FROM CRM_Targets AS CT WITH(NOLOCK) WHERE CT.Brand=@Brand AND CT.Type = @Type AND CT.Date= @TempDate

			IF @@ROWCOUNT = 0 AND @TempTarget <>-1
			BEGIN
				INSERT INTO CRM_Targets(Type,Value,Date,Brand,TargetPeriod,LastUpdatedDate) 
				SELECT @Type,Targets,TG_Date,@Brand,1,@CreatedOn FROM @Targets WHERE Id = @i
			END
			ELSE
			BEGIN
				SELECT ID FROM CRM_Targets AS CT WITH(NOLOCK) WHERE CT.Brand=@Brand AND CT.Type = @Type AND CT.Date= @TempDate AND CT.Value=@TempTarget
				IF @@ROWCOUNT = 0
				BEGIN
					UPDATE CRM_Targets SET Value=@TempTarget,LastUpdatedDate=@CreatedOn WHERE Brand=@Brand AND Type = @Type AND Date= @TempDate
				END  
			END 

			SET @i = @i + 1
			SET @NumberOfTargets = @NumberOfTargets - 1
		END 
		SET @Status = 1    		   
  END 
