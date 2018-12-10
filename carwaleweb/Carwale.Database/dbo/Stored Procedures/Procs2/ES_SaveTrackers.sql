IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_SaveTrackers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_SaveTrackers]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 10th October 2014
-- Description : Save Enterprise sale Trackers data.
-- =============================================

CREATE PROCEDURE [dbo].[ES_SaveTrackers]
    (
	@TrackersTypeId	   INT,
	@TrackersTypeName  Varchar(50),
	@TrackersCount     Float,
	@TrackersMonthYear Date,
	@weekendDate       Date,
	@WeekNoYear        TinyInt,
	--@WeekNoMonth       TinyInt, -- No Use Of This colomn in table ES_TrackerData 
	@CreatedBy         INT,
	@CreatedOn         DateTime,
	@Type              TINYINT,
	@Status		       TINYINT OUTPUT
	)
 AS
 DECLARE @TrackersID INT

   BEGIN
   IF @TrackersTypeId <> -1
		BEGIN
		SELECT ES_Trackers_Id FROM ES_TrackerData  WHERE WeekendDate=@weekendDate AND ES_Trackers_Id=@TrackersTypeId AND Type=@Type
	    	
		IF @@ROWCOUNT > 0
			BEGIN
			    UPDATE ES_TrackerData SET TrackerCount=@TrackersCount 
				WHERE WeekendDate=@weekendDate AND ES_Trackers_Id=@TrackersTypeId AND Type=@Type
				SET  @Status = 2
			END
		ELSE
			BEGIN
				INSERT INTO ES_TrackerData(ES_Trackers_Id,TrackerCount,TrackerMonthYear,WeekendDate,WeekNoYear,CreatedBy,CreatedOn,Type)
				VALUES(@TrackersTypeId,@TrackersCount,@TrackersMonthYear,@weekendDate,@WeekNoYear,@CreatedBy,@CreatedOn,@Type)
				SET  @Status = 1
			END		
		END
   ELSE
        BEGIN

			SELECT ET.ES_Trackers_Id FROM  ES_Trackers AS ET WITH(NOLOCK) WHERE  ET.Name = @TrackersTypeName 
	    	
			IF @@ROWCOUNT > 0
				BEGIN
				SET  @Status = 0
				END
            ELSE
				BEGIN
					INSERT INTO ES_Trackers(Name) VALUES(@TrackersTypeName)
					SET @TrackersID = SCOPE_IDENTITY()

					INSERT INTO ES_TrackerData(ES_Trackers_Id,TrackerCount,TrackerMonthYear,WeekendDate,WeekNoYear,CreatedBy,CreatedOn,Type)
					VALUES(@TrackersID,@TrackersCount,@TrackersMonthYear,@weekendDate,@WeekNoYear,@CreatedBy,@CreatedOn,@Type)
					SET  @Status = 1
               END
		END           
    END





