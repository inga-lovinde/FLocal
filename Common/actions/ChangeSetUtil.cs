using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core.DB;

namespace FLocal.Common.actions {
	class ChangeSetUtil {

		public static void WithChangeSet(Action<ChangeSet, Transaction> action) {
			using(ChangeSet changeSet = new ChangeSet()) {
				Config.Transactional(transaction => {
					action(changeSet, transaction);
					changeSet.Apply(transaction);
				});
			}
		}

		public static void WithChangeSet(Action<ChangeSet> action) {
			WithChangeSet((changeset, transaction) => action(changeset));
		}

		public static void ApplyChanges(IEnumerable<AbstractChange> changes) {
			using(ChangeSet changeSet = new ChangeSet()) {
				foreach(AbstractChange change in changes) {
					changeSet.Add(change);
				}
				Config.Transactional(transaction => {
					changeSet.Apply(transaction);
				});
			}
		}

	}
}
