IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_INSERT_MonthlyPlan]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_INSERT_MonthlyPlan]
GO

	

-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 9th Aug 2011
-- Description:	This proc insert data in CRM_MonthlyPlan table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_INSERT_MonthlyPlan]
	
	(
	@Make			Numeric,
	@Days			Int,
	@Month			VARCHAR(5),
	@Year			VARCHAR(5),
	@UserName		VARCHAR(50),
	@LeadsinCRM		VARCHAR(2000),
	@LeadVerifd		VARCHAR(2000),
	@LeadConsltant	VARCHAR(2000),
	@LeadAssign		VARCHAR(2000),
	@PQReq			VARCHAR(2000),	
	@PQDeliver		VARCHAR(2000),
    @TDReq			VARCHAR(2000),
    @TDDelver		VARCHAR(2000),
    @BookReq			VARCHAR(2000),
    @BookComp		VARCHAR(2000)
	)
	AS
	BEGIN
			SET @LeadsinCRM='5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5'
	
			DECLARE @LeadsinCRMIndx SMALLINT,@strLeadsinCRM VARCHAR(10),@LeadVerifdIndx SMALLINT,@strLeadVerifd VARCHAR(10),@LeadConsltantIndx SMALLINT,
					@strLeadConsltant VARCHAR(10),@LeadAssignIndx SMALLINT,@strLeadAssign VARCHAR(10),@PQReqIndx SMALLINT,@strPQReq VARCHAR(10),
					@PQDeliverIndx SMALLINT,@strPQDeliver VARCHAR(10),@TDReqIndx SMALLINT,@strTDReq VARCHAR(10),
					@BookReqIndx SMALLINT,@strBookReq VARCHAR(10),@TdDelverIndx SMALLINT,@strTdDelver VARCHAR(10),@BookCompIndx SMALLINT,@strBookComp VARCHAR(10),
					@Count int,@Dt VARCHAR(12),@Date DateTime 
			
			SET @Count=1
			
			WHILE @Count <= @Days                 
				BEGIN                
						SET @LeadsinCRMIndx = CHARINDEX(',', @LeadsinCRM) 
						SET @LeadVerifdIndx = CHARINDEX(',', @LeadVerifd) 
						SET @LeadConsltantIndx = CHARINDEX(',', @LeadConsltant) 
						SET @LeadAssignIndx = CHARINDEX(',', @LeadAssign) 
						SET @PQReqIndx = CHARINDEX(',', @PQReq) 
						SET @PQDeliverIndx = CHARINDEX(',', @PQDeliver) 
						SET @TDReqIndx = CHARINDEX(',', @TDReq) 
						SET @TdDelverIndx = CHARINDEX(',', @TDDelver)
						SET @BookReqIndx = CHARINDEX(',', @BookReq)
					    SET @BookCompIndx = CHARINDEX(',', @BookComp)
					    
						SET @Dt = @Month + '/' + CAST(@Count AS VARCHAR(3))+ '/' + @Year
						SET @Date = CAST(@Dt AS DateTime)
						
				--to check if list id has ended                
						IF @LeadsinCRMIndx>0                
							BEGIN  
            
								SET @strLeadsinCRM = LEFT(@LeadsinCRM, @LeadsinCRMIndx-1)
								SET @LeadsinCRM = RIGHT(@LeadsinCRM,LEN(@LeadsinCRM)-@LeadsinCRMIndx) 
								
								SET @strLeadVerifd = LEFT(@LeadVerifd, @LeadVerifdIndx-1)   
								SET @LeadVerifd = RIGHT(@LeadVerifd, LEN(@LeadVerifd)-@LeadVerifdIndx) 
				                
								SET @strLeadConsltant = LEFT(@LeadConsltant, @LeadConsltantIndx-1)  
								SET @LeadConsltant = RIGHT(@LeadConsltant, LEN(@LeadConsltant)-@LeadConsltantIndx)
				                
								SET @strLeadAssign = LEFT(@LeadAssign, @LeadAssignIndx-1)  
								SET @LeadAssign = RIGHT(@LeadAssign, LEN(@LeadAssign)-@LeadAssignIndx)
				                
								SET @strPQReq = LEFT(@PQReq, @PQReqIndx-1)   
								SET @PQReq = RIGHT(@PQReq, LEN(@PQReq)-@PQReqIndx)
				                
								SET @strPQDeliver = LEFT(@PQDeliver, @PQDeliverIndx-1)   
								SET @PQDeliver = RIGHT(@PQDeliver, LEN(@PQDeliver)-@PQDeliverIndx)
				                
								SET @strTDReq = LEFT(@TDReq, @TDReqIndx-1)   
								SET @TDReq = RIGHT(@TDReq, LEN(@TDReq)-@TDReqIndx)
								
								SET @strTdDelver = LEFT(@TDDelver, @TdDelverIndx-1)   
								SET @TDDelver = RIGHT(@TDDelver, LEN(@TDDelver)-@TdDelverIndx)
								
								SET @strBookReq = LEFT(@BookReq, @BookReqIndx-1)   
								SET @BookReq = RIGHT(@BookReq, LEN(@BookReq)-@BookReqIndx)
								
								SET @strBookComp = LEFT(@BookComp, @BookCompIndx-1)   
								SET @BookComp = RIGHT(@BookComp, LEN(@BookComp)-@BookCompIndx)
								
							    Insert Into CRM_MonthlyPlan
								(
									Plan_Date,MakeId,LeadsInCrm,LeadsVerified,LeadsAssign,LeadsToConsultant,PQ_Req, PQ_Delivr,TD_Req,TD_Delivr,Booking_Req,Booking_Comp,Created_By
								)
								VALUES
								(
									@Date,@Make,@strLeadsinCRM,@strLeadVerifd,@strLeadAssign,@strLeadConsltant,@strPQReq,@strPQDeliver,@strTDReq,@strTdDelver,@strBookReq,@strBookComp,@UserName
								)
								
							SET @Count=@Count+1
							END
					END
							               
						
END
						
							
	
	
	

