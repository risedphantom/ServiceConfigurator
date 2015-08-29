CREATE proc [dbo].[GetFailedRestrictions]
as
begin	
	declare @Now datetime = getdate()

	select	IR.ID
		,	TypeName
		,	[Rule]
		,	S.Hostname
		,	S.SiteName
		,	S.[Group]
		,	IR.StartMoment
		,	IR.StopMoment
		,	IR.CreateMoment
		,	IR.Error
	from	IISRestriction IR
			join IISSite S on IR.IISSiteID = S.ID
	where	Error is not null
	order by IR.StartMoment desc
end