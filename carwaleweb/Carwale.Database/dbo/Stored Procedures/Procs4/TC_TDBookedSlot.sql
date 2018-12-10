IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDBookedSlot]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDBookedSlot]
GO

	-- =======================================================================    
-- Author:  Tejashree Patil on 10 Dec 2012 at 11 am 
-- Description: Return details report of scheduled testdrive request  
-- EXEC [TC_TDBookedSlot] 5,'2012-12-30',46,1,1
-- Modified By: Nilesh Utture on 28/12/2012, Added UNION condition and Added parameter @UserId
-- Modified By: Tejashree Patil on 3 Jan 2012, Checked that TDStatus should not cancel
-- Modified By: Nilesh Utture on 22/01/2013, Added AreaName in SELECT statement
-- EXEC [TC_TDBookedSlot] 5,'12/24/2012',47,1,1
-- =======================================================================
CREATE PROCEDURE [dbo].[TC_TDBookedSlot]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
	@TDDate DATE,
	@TDCarId BIGINT,
	@OtherCars BIT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    -- @OtherCars=0 for displaying data for input car version only
	IF(@OtherCars = 0)
	BEGIN
		SELECT                        
				-- TDSchedule details  
				C.TC_TDCalendarId 'TDScheduleId', C.TC_TDCarsId 'CarId',C.TDCarDetails 'CarDetails', C.AreaName,-- Modified By: Nilesh Utture on 22/01/2013
				C.TDStatus 'StatusId',Status	= (CASE C.TDStatus	WHEN 1 THEN 'Tentative' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'Complete' WHEN 4 THEN 'Cancelled' END ),
				CONVERT(VARCHAR(10),C.TDDate,101) 'TDDate',CONVERT(VARCHAR(10),C.TDStartTime,100) 'StartTime' , CONVERT(VARCHAR(10),C.TDEndTime,100) as 'EndTime'
		 
		FROM	TC_TDCalendar C WITH(NOLOCK)  
				INNER JOIN	TC_TDCars TDC WITH(NOLOCK)
							ON TDC.TC_TDCarsId=C.TC_TDCarsId 
		WHERE	C.BranchId=@BranchId
				AND (C.TDDate = @TDDate) 
				-- Modified By: Tejashree Patil on 3 Jan 2012
				AND C.TDStatus NOT IN (0,4) -- 0 for default status
				AND TDC.IsActive=1
				AND TDC.TC_TDCarsId=@TDCarId
		-- Modified By: Nilesh Utture on 28/12/2012		
		UNION
		
		SELECT                        
				-- TDSchedule details  
				C.TC_TDCalendarId 'TDScheduleId', C.TC_TDCarsId 'CarId',C.TDCarDetails 'CarDetails',  C.AreaName,-- Modified By: Nilesh Utture on 22/01/2013
				C.TDStatus 'StatusId',Status	= (CASE C.TDStatus	WHEN 1 THEN 'Tentative' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'Complete' WHEN 4 THEN 'Cancelled' END ),
				CONVERT(VARCHAR(10),C.TDDate,101) 'TDDate',CONVERT(VARCHAR(10),C.TDStartTime,100) 'StartTime' , CONVERT(VARCHAR(10),C.TDEndTime,100) as 'EndTime'
		 
		FROM	TC_TDCalendar C WITH(NOLOCK)  
				INNER JOIN	TC_TDCars TDC WITH(NOLOCK)
							ON TDC.TC_TDCarsId=C.TC_TDCarsId 
		WHERE	C.BranchId=@BranchId
				AND (C.TDDate = @TDDate) 
				AND C.TC_UsersId = @UserId
				-- Modified By: Tejashree Patil on 3 Jan 2012
				AND C.TDStatus NOT IN (0,4)
	END
	ELSE
	-- @OtherCars=1 for displaying data for all versions of @TDCars's modelId
	BEGIN	
		DECLARE @tblVersion TABLE(VersionId INT)
		INSERT INTO @tblVersion (VersionId)
		SELECT Id 
		FROM CarVersions CV
		WHERE CarModelId = (SELECT	CarModelId 
							FROM	CarVersions CV WITH(NOLOCK)
									INNER JOIN	TC_TDCars TDC WITH(NOLOCK)
												ON TDC.VersionId=CV.Id
							WHERE	TDC.TC_TDCarsId=@TDCarId) 
									--AND CV.New=1 AND IsDeleted=0)
		SELECT
				-- TDSchedule details				
				C.TC_TDCalendarId 'TDScheduleId', C.TC_TDCarsId 'CarId',C.TDCarDetails 'CarDetails',  C.AreaName,-- Modified By: Nilesh Utture on 22/01/2013
				C.TDStatus 'StatusId',Status	= (CASE C.TDStatus	WHEN 1 THEN 'Tentative' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'Complete' WHEN 4 THEN 'Cancelled' END ),
				CONVERT(VARCHAR(10),C.TDDate,101) 'TDDate',CONVERT(VARCHAR(10),C.TDStartTime,100) 'StartTime' , CONVERT(VARCHAR(10),C.TDEndTime,100) as 'EndTime'

		FROM	TC_TDCalendar C WITH(NOLOCK)
				INNER JOIN	TC_TDCars TDC WITH(NOLOCK)
							ON TDC.TC_TDCarsId=C.TC_TDCarsId -- Removed INNER JOIN with vwMMV as it was not necessary
		WHERE	C.BranchId=@BranchId
				AND (C.TDDate = @TDDate) 
				-- Modified By: Tejashree Patil on 3 Jan 2012
				AND C.TDStatus NOT IN (0,4) -- 0 for default status
				AND TDC.IsActive=1
				AND TDC.VersionId IN (SELECT VersionId FROM @tblVersion)
		-- Modified By: Nilesh Utture on 28/12/2012
		UNION
		
		SELECT                        
				-- TDSchedule details  
				C.TC_TDCalendarId 'TDScheduleId', C.TC_TDCarsId 'CarId',C.TDCarDetails 'CarDetails',  C.AreaName,-- Modified By: Nilesh Utture on 22/01/2013
				C.TDStatus 'StatusId',Status	= (CASE C.TDStatus	WHEN 1 THEN 'Tentative' WHEN 2 THEN 'Confirmed' WHEN 3 THEN 'Complete' WHEN 4 THEN 'Cancelled' END ),
				CONVERT(VARCHAR(10),C.TDDate,101) 'TDDate',CONVERT(VARCHAR(10),C.TDStartTime,100) 'StartTime' , CONVERT(VARCHAR(10),C.TDEndTime,100) as 'EndTime'
		 
		FROM	TC_TDCalendar C WITH(NOLOCK)  
				INNER JOIN	TC_TDCars TDC WITH(NOLOCK)
							ON TDC.TC_TDCarsId=C.TC_TDCarsId 
		WHERE	C.BranchId=@BranchId
				AND (C.TDDate = @TDDate) 
				AND C.TC_UsersId = @UserId
				-- Modified By: Tejashree Patil on 3 Jan 2012
				AND C.TDStatus NOT IN (0,4)
	END
END
