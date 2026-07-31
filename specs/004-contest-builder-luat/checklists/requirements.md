# Specification Quality Checklist: Contest builder và luật tuỳ biến

**Purpose**: Validate specification completeness and quality before proceeding to planning
**Created**: 2026-07-30
**Feature**: [spec.md](../spec.md)

## Content Quality

- [x] No implementation details (languages, frameworks, APIs)
- [x] Focused on user value and business needs
- [x] Written for non-technical stakeholders
- [x] All mandatory sections completed

## Requirement Completeness

- [x] No [NEEDS CLARIFICATION] markers remain — all 5 (Q1–Q5) resolved in the 2026-07-30 clarification session and recorded in §Clarifications; none answered by inference.
- [x] Requirements are testable and unambiguous (each FR-NNN traces to US/PRD-REQ/GR/AC)
- [x] Success criteria are measurable
- [x] Success criteria are technology-agnostic (no implementation details)
- [x] All acceptance scenarios are defined (Given/When/Then, incl. no-change guarantees on rejection paths)
- [x] Edge cases are identified (boundary · invalid state · repeated action ×2 · stale state · duplicate event · partial failure · conflicting rule · missing source behavior ×2)
- [x] Scope is clearly bounded (§Phạm vi, §Out of Scope list adjacent epics and non-goals by ID)
- [x] Dependencies and assumptions identified (§Assumptions; EPIC-002 kho đề as precondition)

## Feature Readiness

- [x] All functional requirements have clear acceptance criteria (FR↔AC cross-references throughout)
- [x] User scenarios cover primary flows (7 user stories, P1×5, P2×1, P3×1)
- [x] Feature meets measurable outcomes defined in Success Criteria
- [x] No implementation details leak into specification

## Notes

- Two independent validation passes (traceability-vs-source audit, structural-compliance audit) were run against the source docs and the command's authoring rules after the first draft. Findings applied: fixed a Q1/Q2 open-question mislabel in US-004; added FR-027 + AC US2-8/US2-9 to cover PRD-REQ-022 (non-O26 `stealMode`/`exhaustedFallback` options), which the first draft omitted entirely; attached Actor/Intent/User value/Related PRD/GR/journey metadata to every user story instead of only the tail-end traceability matrix; strengthened rejection-path Thens with explicit no-change clauses; added missing GR/PRD citations.
- `/speckit-clarify` session (2026-07-30) resolved all 5 markers: Q1 (contest-level RuleConfig/question-list edit window — resolved by extending `GR-031` C8, parallel to `QĐ-075`), Q2 (duplicate seat position → "replace?" dialog; ≠4 seats → **originally** a hard block at seat-assignment, **since corrected — see the 2026-07-30 PRD-reconciliation note below**) and its follow-on Q5 (≠4 seats on a **practice** contest → same treatment as official), Q3 (out-of-lock `rowCount`/`tieBreakPositions` → server-side zero-trust rejection, not just a UI convention; this also corrected FR-005/FR-008's original wording, which had understated the server's role), Q4 (theme/sound is optional (SHOULD), with a preset-pack mechanism reconciled against `CLAUDE.md` §Sound's "no default SFX" rule). New FRs added during integration: FR-028 through FR-032.
- The `docs/rule-traceability.md` / `docs/reviews/prd-review.md` path note at the top of spec.md, and the still-open PRD traceability gap for theme/sound (no `PRD-REQ` id — flagged for the project owner to add to `docs/PRD.md` §12, not blocking this spec), are the only remaining items outside this checklist's scope. Everything else passes.
- **Correction (2026-07-30, spec ↔ PRD reconciliation pass)**: the Q2/Q5 answer *"hard block ≠4 seats at the seat-assignment step"* **contradicted `docs/PRD.md`** and has been replaced. `PRD-REQ-026` requires *"mô hình dữ liệu **và giao diện** MUST hỗ trợ 1-12 ghế"*; `NON-GOAL-012` scopes out only the **rule path**, explicitly keeping storage **and UI** at 1-12; `QĐ-007` says the same and promises v1.5 ships *"luật + sửa controller"* with no UI rebuild. Seat count is therefore **not** a fourth v1 hard lock. FR-030 rewritten (seat assignment accepts 1-12 for official and practice alike; the hard block moves to the **match-start gate**), FR-013 restored to include the UI clause, AC US4-1/US4-2/US4-2b/US4-5 rewritten, §Out of Scope split `NON-GOAL-012` away from the three genuine hard locks, and a clarifying `Note` added to `PRD-REQ-026`. New **Q6** logged: which spec owns the counterpart FR at the match-start gate (`PRD-REQ-028`/EPIC-005) — a scope-assignment gap between specs 004 and 005, not an ambiguity inside this spec, so §Requirement Completeness stays green.
- **Reconciliation of Q3 with the PRD (2026-07-30)**: the Q3 reading (schema capacity ≠ a valid write path; server rejects `rowCount ≠ 4` / `tieBreakPositions ≠ [1]`) was confirmed as correct and **propagated** — `specs/005-phong-thi-vong-doi-tran` FR-017 had kept the looser PRD wording and now matches FR-008, and clarifying `Note`s were added to `PRD-REQ-025` and `PRD-REQ-105`.
- Follow-up (2026-07-30, direct project-owner request, not through `/speckit-clarify`): generalized the zero-trust posture from Q3 into a cross-cutting **FR-033** — every block/lock/validate rule in this spec MUST be server-enforced, with UI-side blocking treated only as a fast-feedback enhancement, never the sole gate. Updated FR-004, FR-006, FR-007, FR-014, FR-016, FR-018, FR-019, FR-022, FR-028, FR-029 to point at FR-033 instead of repeating the same "server MUST" clause (DRY, per `CLAUDE.md` §Nguyên tắc code), and logged the decision in §Clarifications.
